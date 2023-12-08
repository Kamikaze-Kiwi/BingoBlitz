# Kubernetes guide

This guide will help you set up a local Kubernetes environment for this project. 

# Requirements

- Docker installed
- Docker-compose installed
- Kubectl installed
- Minikube installed
- *(Step 0 only)* Kompose installed

# Commands

*(in order, run in the project root folder (where this file is located))*

0. *(Optional, only needed when docker-compose.yml edited)* Convert docker-compose to Kubernetes configurations:
    ```
    docker compose config > docker-compose-resolved.yml
    kompose convert -f docker-compose-resolved.yml -o .kubernetes
    ```

1. Start Minikube:
    ```
    minikube start
    ```

2. Set up Minikube for Docker environment in PowerShell:
    ```
    minikube -p minikube docker-env --shell powershell | Invoke-Expression
    ```

3. Build Docker Compose:
    ```
    docker-compose build
    ```

4. Push Docker Compose:
    ```
    docker-compose push
    ```

5. Apply Kubernetes configurations:
    ```
    kubectl apply -f .kubernetes
    ```

6. *(Separate terminal)* Open kubernetes tunnel:
    ```
    minikube tunnel
    ```

7. *(Optional, separate terminal)* Open Minikube Dashboard:
    ```
    minikube dashboard
    ```

8. *(Optional)* Get service status and IP/ports:
    ```
    kubectl get services
    ```