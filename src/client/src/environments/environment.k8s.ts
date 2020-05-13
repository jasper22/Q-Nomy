export const environment = {
  production: true,
  // This is Kubernetes address as defined in webapi-service.yaml
  serverUrl: 'http://webapi-k8s.default.svc.cluster.local:5007/api/v1/Patients'
};
