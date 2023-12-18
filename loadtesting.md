# Load testing guide

This guide will help you set up load testing for this project. This guide assumes that you have already set up a local Kubernetes environment for this project. If you have not done so, please refer to the [kubernetes guide](kubernetes.md) for more information.

To get more detailed live feedback on the load testing, you can use the [observing guide](observing.md) to set up Prometheus and Grafana.


## Requirements

- k6 must be installed on your machine. You can download k6 [here](https://k6.io/docs/getting-started/installation/).
- The project must be running in a local Kubernetes environment (see [Kubernetes guide](kubernetes.md))


## Running load tests

To run a load test, run the following command:

```sh
k6 run .loadtesting/<TEST_NAME>.js
```


## Creating new load tests

Run the following command to create a new load test:

```sh
k6 new .loadtesting/<YOUR_TEST_NAME>.js
```