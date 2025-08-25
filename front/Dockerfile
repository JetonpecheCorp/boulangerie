FROM node:current-alpine as build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --prod

FROM nginx:alpine
COPY  --from=build /app/dist/front/browser /usr/share/nginx/html