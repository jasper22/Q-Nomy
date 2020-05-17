# Build Angular
FROM docker.local:5000/node:latest as build_angular
WORKDIR /client
COPY package.json .
RUN npm install
COPY . .
# ARG configuration=production
ARG configuration=kubernetes
RUN npm run build -- --configuration=${configuration} --outputPath=./dist/out --deleteOutputPath=true --extractCss=true --aot=true --buildOptimizer=true

# Build ngnix image
FROM docker.local:5000/nginx:1.17.10 as nginx
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build_angular /client/dist/out/ /usr/share/nginx/html
COPY ngnix-custom.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
LABEL "qnomy"="frontend"
