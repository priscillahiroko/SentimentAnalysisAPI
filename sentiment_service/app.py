import os
from fastapi import FastAPI, HTTPException, Request
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from textblob import TextBlob

# ========== Models ==========
class InputRequest(BaseModel):
    text: str

class SentimentResult(BaseModel):
    text: str
    polarity: float
    subjectivity: float
    detected_language: str
    translated_text: str
    word_count: int
    sentence_count: int
    estimated_reading_time: float

# ========== Init app ==========
API_KEY = os.getenv("API_KEY")
if not API_KEY:
    raise RuntimeError("A variável de ambiente API_KEY não está definida.")

app = FastAPI(title="sentiment_service",
              description="Serviço de análise de sentimento com FastAPI & TextBlob",
              version="1.0.0")

# (opcional) habilita CORS se for chamado de browser em outra origem
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

# ========== Middleware de autenticação ==========
@app.middleware("http")
async def require_api_key(request: Request, call_next):
    # Liberar Swagger, ReDoc e OpenAPI
    if request.url.path in ["/docs", "/redoc", "/openapi.json"]:
        return await call_next(request)
    key = request.headers.get("x-api-key")
    if key != API_KEY:
        raise HTTPException(status_code=401, detail="Unauthorized: API key inválida.")
    return await call_next(request)


# ========== Endpoints ==========
@app.get("/health")
async def health():
    """
    Health check simples.
    """
    return {"status": "ok"}

@app.post("/analyze", response_model=SentimentResult)
async def analyze(req: InputRequest):
    """
    Analisa um texto (sentimento, linguagem, tradução, contagens, tempo de leitura).
    """
    blob = TextBlob(req.text)
    detected_lang = blob.detect_language()
    # se o texto não estiver em inglês, traduz para inglês para extrair polaridade/subjectivity
    tb_en = blob if detected_lang == "en" else blob.translate(to="en")

    polarity = tb_en.sentiment.polarity
    subjectivity = tb_en.sentiment.subjectivity
    translated_text = str(tb_en)

    words = req.text.split()
    sentences = req.text.split(".")
    word_count = len(words)
    sentence_count = len([s for s in sentences if s.strip()])

    # tempo estimado de leitura em minutos (200 wpm)
    estimated_reading_time = word_count / 200

    return {
        "text": req.text,
        "polarity": polarity,
        "subjectivity": subjectivity,
        "detected_language": detected_lang,
        "translated_text": translated_text,
        "word_count": word_count,
        "sentence_count": sentence_count,
        "estimated_reading_time": estimated_reading_time,
    }

@app.post("/analyze/batch", response_model=list[SentimentResult])
async def analyze_batch(requests: list[InputRequest]):
    """
    Analisa uma lista de textos em batch.
    """
    results = []
    for r in requests:
        res = await analyze(r)
        results.append(res)
    return results
