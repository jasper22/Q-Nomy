#!/bin/bash

echo '-- Please make sure that kubectl installed and running ---------------------'
echo '-- Please make sure that minikube configured and cluster created -----------'
echo '-- Please make sure that Docker enviroment pointing to minikube registry ---'
echo '\n\n'


echo '-- Build client ------------------------------------------------------------'
docker build -t clientapp:1.0 -f src/client/webclient.Dockerfile src/client/

echo '-- Deploy Angular client to Kubernetes -------------------------------------'
kubectl apply -f .k8s/clientapp-deployment.yaml
kubectl apply -f .k8s/clientapp-service.yaml


echo '-- Build WebAPI ------------------------------------------------------------'
docker build -t webapi:1.0 -f src/server/webapi.Dockerfile src/server

echo '-- Deploy WebAPI to Kubernetes ---------------------------------------------'
kubectl apply -f .k8s/webapi-deployment.yaml
kubectl apply -f .k8s/webapi-service.yaml


echo '-- Build SQL server --------------------------------------------------------'
docker build -t sqldatabase:1.0 -f src/server/sql.Dockerfile src/server

echo '-- Deploy SQL server to Kuberenetes ----------------------------------------'
kubectl apply -f .k8s/sqldatabase-deployment.yaml
kubectl apply -f .k8s/sqldatabase-service.yaml

echo '-- All done ----------------------------------------------------------------'
