# Resume Scan Product API

## Endpoints

All Product endpoints require Basic Authentication. Configure `BasicAuthentication:Username` and `BasicAuthentication:Password` in `ResumeScanApi/appsettings.json` or a secret/environment-specific configuration source.

- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`

Example request body:

```json
{"name":"Professional Resume Review","price":49.99,"quantity":10}
```

Example response:

```json
{"success":true,"message":"Product created successfully.","data":{"id":1,"name":"Professional Resume Review","price":49.99,"quantity":10},"errors":null}
```

Run `Database/Product.sql` against SQL Server, update the connection string, then start the API and open `/swagger`. Use the Swagger **Authorize** button with the configured username and password.

Engineer registration uploads document contents with the registration request. The API saves each file in the `EngineerDocuments` directory under its content root using a generated file name, and stores the relative path and file metadata in SQL Server. PDF, JPG, JPEG, and PNG files are accepted; each file is limited to 10 MB, with a 20 MB total limit per registration.

Retrieve an engineer's attachment list with `GET /api/field-service-engineers/{fieldServiceEngineerId}/documents`. Each item includes a `downloadUrl` that downloads that individual file, for example `GET /api/field-service-engineers/12/documents/34/download`. Both endpoints require Basic Authentication.
