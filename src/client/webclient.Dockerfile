# Build Angular
FROM node:latest as build_angular
WORKDIR /client
COPY package.json .
RUN npm install
COPY . .
ARG configuration=production
RUN npm run build -- --outputPath=./dist/out --configuration=${configuration}

# Build ngnix image
FROM nginx:1.17.10 as nginx
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build_angular /client/dist/out/ /usr/share/nginx/html
COPY ngnix-custom.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
LABEL "qnomy"="frontend"
