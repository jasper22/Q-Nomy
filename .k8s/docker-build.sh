#!/bin/bash

echo '-- Please make sure that kubectl installed and running ---------------------'
echo '-- Please make sure that minikube configured and cluster created -----------'
echo '-- Please make sure that Docker enviroment pointing to minikube registry ---'
echo '\n\n'

export DOCKER_REGISTRY_HOST="docker.local:5000"

# docker-compose -f ../docker-compose.yml build --parallel --build-arg DOCKER_REGISTRY_HOST=${DOCKER_REGISTRY_HOST}
# docker-compose push

echo '-- Build client ------------------------------------------------------------'
docker build -t $DOCKER_REGISTRY_HOST/clientapp:1.0 -f ../src/client/webclient.Dockerfile --build-arg DOCKER_REGISTRY_HOST=${DOCKER_REGISTRY_HOST} ../src/client/
docker push $DOCKER_REGISTRY_HOST/q-nomy/clientapp:1.0

echo '-- Build WebAPI ------------------------------------------------------------'
docker build -t $DOCKER_REGISTRY_HOST/webapi:1.0 -f ../src/server/webapi.Dockerfile --build-arg DOCKER_REGISTRY_HOST=${DOCKER_REGISTRY_HOST} ../src/server
docker push $DOCKER_REGISTRY_HOST/webapi:1.0

echo '-- Build SQL server --------------------------------------------------------'
docker build -t $DOCKER_REGISTRY_HOST/sqldatabase:1.0 -f ../src/server/sql.Dockerfile --build-arg DOCKER_REGISTRY_HOST=${DOCKER_REGISTRY_HOST} ../src/server
docker push $DOCKER_REGISTRY_HOST/sqldatabase:1.0

echo '-- Deploy to Kuberenetes ----------------------------------------'
kubectl apply -f .k8s/

echo '-- Adding host addresses to local computer ---------------------------------'
echo '$(minikube ip) qnomy-local webapi-k8s.qnomy-local' | sudo tee -a /etc/hosts

echo '-- All done ----------------------------------------------------------------'
