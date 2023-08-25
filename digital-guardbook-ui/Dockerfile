FROM node:20-bookworm AS builder
WORKDIR /app
COPY package.json package-lock.json /app/
RUN npm ci
COPY vite.config.ts tsconfig.* index.html apollo.config.js src/ public/ /app/
RUN npm run build

FROM nginx:1.25.2
COPY --from=builder /app/dist/ /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf