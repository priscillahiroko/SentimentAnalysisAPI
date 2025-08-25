Sentiment Analysis Microservices

Um projeto de arquitetura de microserviços para análise de sentimento, utilizando Python (FastAPI) para o processamento de linguagem natural e .NET como API Gateway, tudo orquestrado com Docker.

📝 Descrição
Este projeto demonstra uma abordagem desacoplada para análise de sentimento. Ele consiste em dois serviços principais:

sentiment_service (Python/FastAPI): O núcleo do projeto. Um serviço que recebe um texto, detecta o idioma, traduz se necessário, e retorna a polaridade (positivo, neutro, negativo) e a subjetividade (opinativo vs. fatual).

SentimentAnalysisAPI (.NET): Atua como uma API Gateway, consumindo o serviço Python e expondo os resultados de forma estruturada.

A comunicação entre os serviços e a gestão do ambiente são feitas inteiramente com Docker, garantindo portabilidade e facilidade na execução.

✨ Funcionalidades

Análise de Sentimento: Classifica a polaridade e a subjetividade do texto.

Detecção de Idioma: Identifica o idioma do texto de entrada.

Estatísticas de Texto: Fornece contagem de palavras, sentenças e tempo estimado de leitura.

API RESTful: Endpoints claros e bem documentados com Swagger / OpenAPI.

Segurança: Endpoint de análise protegido por chave de API (API Key).

Containerização: Totalmente containerizado com Docker e orquestrado com Docker Compose.

🛠️ Tecnologias Utilizadas

Backend (Análise): Python 3.9, FastAPI, TextBlob, Uvicorn

Backend (Gateway): .NET 6

Containerização: Docker, Docker Compose

🚀 Como Executar o Projeto
Siga os passos abaixo para executar a aplicação completa em seu ambiente local.

Pré-requisitos

Git

Docker

Docker Compose

1. Clone o Repositório
git clone https://github.com/priscillahiroko/SentimentAnalysisAPI
cd SEU-REPOSITORIO


2. Inicie os Serviços com Docker Compose
Na raiz do projeto, execute o comando abaixo para construir as imagens e iniciar os containers em modo detached (-d).

docker-compose up --build -d

3. Verifique os Serviços
Após a execução, os seguintes serviços estarão disponíveis:

API .NET (Gateway)

Swagger UI: http://localhost:5000/swagger

Serviço de Análise (Python)

Swagger UI: http://localhost:8000/docs

4. Para Parar a Aplicação
Para parar todos os containers, execute:

docker-compose down

⚙️ Uso da API
Autenticação
O serviço de análise (sentiment_service) é protegido por uma chave de API. A chave padrão está definida no arquivo docker-compose.yml e é minha-chave-secreta.

Para usar os endpoints protegidos no Swagger UI (http://localhost:8000/docs):

Clique no botão Authorize (canto superior direito).

No campo "Value", insira minha-chave-secreta.

Clique em Authorize para salvar.

Exemplo de Requisição (cURL)
Você pode testar o serviço de análise diretamente pelo terminal:

curl -X 'POST' \
  'http://localhost:8000/analyze' \
  -H 'accept: application/json' \
  -H 'x-api-key: minha-chave-secreta' \
  -H 'Content-Type: application/json' \
  -d '{
  "text": "FastAPI is an amazing framework! I am loving it."
}'

📄 Licença
Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

Feito com ❤️ por [Priscilla H. S. Pito]