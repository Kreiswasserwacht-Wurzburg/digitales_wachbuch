FROM docker.io/node:20-bookworm AS builder
WORKDIR /app
COPY package.json package-lock.json /app/
RUN npm ci
COPY . /app/
RUN npm run build

FROM docker.io/nginx:1.25.2
COPY --from=builder /app/dist/ /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf