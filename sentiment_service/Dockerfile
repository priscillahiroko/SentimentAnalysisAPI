FROM python:3.9-slim
WORKDIR /app

# Copia e instala dependências
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt && \
    python -m textblob.download_corpora

# Copia o restante do código
COPY . .

EXPOSE 8000

# Usa o modo shell para evitar parsing de JSON mal-formatado
CMD uvicorn app:app --host 0.0.0.0 --port 8000
