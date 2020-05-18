#!/bin/bash

export DOCKER_REGISTRY_HOST="docker.io/library"

docker-compose -f docker-compose.yml build --parallel --build-arg DOCKER_REGISTRY_HOST=${DOCKER_REGISTRY_HOST}