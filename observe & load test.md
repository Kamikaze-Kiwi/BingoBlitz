# Observing & Testing guide

This guide will help you set up observing and load testing for this project. This guide assumes that you have already set up a local Kubernetes environment for this project. If you have not done so, please refer to the [kubernetes guide](kubernetes.md) for more information.


## Requirements

- Docker must be installed and running on your machine
- Docker Compose must be installed on your machine
- Helm must be installed on your machine
- The project must be running in a local Kubernetes environment (see [Kubernetes guide](kubernetes.md))


## Setting up Prometheus

1. Add the Prometheus Helm repository:
    ```sh
    helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
    ```

2. Install Prometheus using Helm:
    ```sh
    helm install prometheus prometheus-community/prometheus
    ```

3. Expose the Prometheus service via NodePort:
    ```sh
    kubectl expose service prometheus-server --type=NodePort --target-port=9090 --name=prometheus-server-np
    ```

4. Expose the service URL:
    ```sh
    minikube service prometheus-server-np
    ```

5. Open the URL in your browser. You should now see the Prometheus dashboard.


## Setting up Grafana

1. Add the Grafana Helm repository:
    ```sh
    helm repo add grafana https://grafana.github.io/helm-charts
    ```

2. Install Grafana using Helm:
    ```sh
    helm install grafana grafana/grafana
    ```

3. Expose the Grafana service via NodePort:
    ```sh
    kubectl expose service grafana --type=NodePort --target-port=3000 --name=grafana-np
    ```

4. Expose the service URL:
    ```sh
    minikube service grafana-np
    ```

5. Get the admin password for Grafana:
    > If you are using Windows, you may need to use Git Bash instead of PowerShell)
    ```sh
    kubectl get secret --namespace default grafana -o jsonpath="{.data.admin-password}" | base64 --decode ; echo
    ```

6. Open the URL in your browser. You should now see the Grafana dashboard. Log in using the username `admin` and the password you got in step 5.


## Integrating Prometheus with Grafana

> Ensure that you have completed the steps in the "Setting up Prometheus" and "Setting up Grafana" sections before continuing.

1. Open the Grafana dashboard in your browser.

2. Open the hamburger menu in the top left corner and click on `Connections`. Then click on `Data sources`, followed by `Add data source`. Here you can select `Prometheus` as the data source type.

3. In the `Connection` section, enter `http://prometheus-server:80` as the URL. Scroll down and click on `Save & Test`. You should now see a green notification saying `Successfully queried the Prometheus API.`.

4. Click on `Build a dashboard` (Either in the green notification, or in the top right corner), then click on `Import dashboard`. Here you can enter the ID of the dashboard you want to import. For this project, I recommend `315`

5. Under `Prometheus` select the Prometheus data source you created in step 3. Then click on `Import`. You should now see the dashboard you imported.

6. (Optional) add the dashboard to your home dashboard by clicking on the star in the top right corner of the dashboard.
