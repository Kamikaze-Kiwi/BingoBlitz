# Requirements

- Docker installed
- Docker-compose installed
- Kubectl installed
- Minikube installed

# Commands (initial setup)

*(in order, run in the project root folder (where this file is located))*

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

6. *(Optional, separate terminal)* Open Minikube Dashboard:
    ```
    minikube dashboard
    ```