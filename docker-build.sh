#!/bin/bash

echo '-- Please make sure that kubectl installed and running ---------------------'
echo '-- Please make sure that minikube configured and cluster created -----------'
echo '-- Please make sure that Docker enviroment pointing to minikube registry ---'
echo '\n\n'

echo '-- Build client ------------------------------------------------------------'
docker build -t docker.local:5000/clientapp:1.0 -f src/client/webclient.Dockerfile src/client/
docker push docker.local:5000/clientapp:1.0

echo '-- Build WebAPI ------------------------------------------------------------'
docker build -t docker.local:5000/webapi:1.0 -f src/server/webapi.Dockerfile src/server
docker push docker.local:5000/webapi:1.0

echo '-- Build SQL server --------------------------------------------------------'
docker build -t docker.local:5000/sqldatabase:1.0 -f src/server/sql.Dockerfile src/server
docker push docker.local:5000/sqldatabase:1.0

echo '-- Deploy to Kuberenetes ----------------------------------------'
kubectl apply -f .k8s/

echo '-- Adding host addresses to local computer ---------------------------------'
echo '$(minikube ip) qnomy-local webapi-k8s.qnomy-local' | sudo tee -a /etc/hosts

echo '-- All done ----------------------------------------------------------------'
