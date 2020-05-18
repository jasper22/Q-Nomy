export const environment = {
  production: true,
  // This is Kubernetes address as defined in webapi-service.yaml
  // Although .NET Core is listening on port number 5000
  // the Ingress controller in Minikube can not handle different port number but 80
  // So redirection is done on Service level and here we asking for port 80
  serverUrl: 'http://webapi-k8s.qnomy-local/api/v1/Patients'
};
