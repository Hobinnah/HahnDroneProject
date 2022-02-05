
FROM node:11.5.0 AS node
WORKDIR /app
COPY . .
RUN npm install 
RUN npm run build --prod

FROM nginx:alpine
COPY --from=node /app/dist/HahnWebClient/ /usr/share/nginx/html

EXPOSE 80

#docker build --rm -t hahnWebClient:latest
