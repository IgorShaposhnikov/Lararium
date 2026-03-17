FROM node:22-alpine AS builder
WORKDIR /app
COPY package*.json .
# Иногда Docker пытается скачивать пакеты через IPv6, который на сервере настроен плохо.
# Пробуем принудительно использовать IPv4.
RUN npm config set registry http://registry.npmjs.org
RUN npm ci
COPY . .
RUN npm run build