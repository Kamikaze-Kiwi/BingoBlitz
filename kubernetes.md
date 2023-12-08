# Kubernetes guide

This guide will help you set up a local Kubernetes environment for this project. 

# Requirements

- Docker installed
- Docker-compose installed
- Kubectl installed
- Minikube installed
- *(Step 0 only)* Kompose installed

# Commands (initial setup)

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


# Commands (after initial setup)
You can copy and paste this into a PowerShell terminal to run all commands at once:

```
minikube start
minikube -p minikube docker-env --shell powershell | Invoke-Expression
docker-compose build
docker-compose push
kubectl apply -f .kubernetes
Start-Process cmd.exe -ArgumentList "/k minikube tunnel"
Start-Process cmd.exe -ArgumentList "/k minikube dashboard"
kubectl get services
```


# Commands (already running, update images)
You can copy and paste this into a PowerShell terminal to run all commands at once:

```
minikube -p minikube docker-env --shell powershell | Invoke-Expression
docker-compose build
docker-compose push
kubectl get services
```