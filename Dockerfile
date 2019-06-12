FROM python:3.7.2-alpine

COPY . /api

WORKDIR /api

RUN pip install -r requirements.txt

EXPOSE 5000

CMD ["python", "/api/src/app.py"]