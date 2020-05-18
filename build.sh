#!/bin/bash

export DOCKER_REGISTRY_HOST="docker.io"

docker-compose -f docker-compose.yml build --build-arg DOCKER_REGISTRY_HOST=${DOCKER_REGISTRY_HOST}