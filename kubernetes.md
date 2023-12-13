# Kubernetes guide

This guide will help you set up a local Kubernetes environment for this project. 

# Requirements

- Docker installed
- Docker-compose installed
- Kubectl installed
- Minikube installed
- bingoblitz domains set up in your hosts file (see [Setting up domains in hosts file](#setting-up-domains-in-hosts-file))

# Setting up domains in hosts file

1. Open hosts file in any text editor as administrator:
    ```
    Windows: C:\Windows\System32\drivers\etc\hosts
    Linux: /etc/hosts
    Mac: /private/etc/hosts
    ```

2. Add the following lines to the end of the file:
    ```
    # Kubernetes Ingress for BingoBlitz
    127.0.0.1 bingoblitz.local
    127.0.0.1 gameserver.bingoblitz.local
    127.0.0.1 communityhub.bingoblitz.local
    127.0.0.1 gameservice.bingoblitz.local
    ```

# Useful commands

*(in order, run in the project root folder (where this file is located))*

1. Start Minikube:
    ```
    minikube start
    ```

2. Set up Minikube for Docker environment in PowerShell (Needed for step 3 & 4):
    ```
    minikube -p minikube docker-env --shell powershell | Invoke-Expression
    ```

3. Build Docker Compose:
    ```
    docker-compose build
    ```

4. Apply Kubernetes configurations:
    ```
    kubectl apply -f .kubernetes
    ```

5. *(Separate terminal)* Open kubernetes tunnel:
    ```
    minikube tunnel
    ```

6. *(Optional, separate terminal)* Open Minikube Dashboard:
    ```
    minikube dashboard
    ```

7. *(Optional)* Get service status and IP/ports:
    ```
    kubectl get services
    ```

# configuration

1. Add community hub env to Kubernetes secrets (you might need to change the values in the .env file to match your Cosmos DB instance)
   ```sh
   kubectl create secret generic communityhub-env --from-env-file=BingoBlitz-CommunityHub/CommunityHubAPI/.env
   ```

# Commands (Start/restart all)
You can copy and paste this into a PowerShell terminal to run all commands at once:

```
minikube start
minikube -p minikube docker-env --shell powershell | Invoke-Expression
docker-compose build
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
kubectl apply -f .kubernetes
```


# Commands (Enable horizontal pod autoscaling)
You can copy and paste this into a PowerShell terminal to run all commands at once:

```
kubectl autoscale deployment gameserver --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment communityhub --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment gameservice --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment frontend --cpu-percent=50 --min=1 --max=10
```