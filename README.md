Diagrama actualizado:
![Diagrama de arquitectura](Untitled Diagram2.drawio.png)

## Endpoints de la API

| ID | MS que Expone | MS que Consume (Cliente) | Endpoint Expuesto (REST API) | Método REST | Descripción | Descripción del Endpoint / Dato Solicitado |
|---|---|---|---|---|---|---|
| 1 | MS1 - Clínica | Front-end (Paciente) | `/api/v1/consultation` | `POST` | Solicita Atención | Crea solicitud de atención inicial |
| 2 | MS1 - Clínica | Front-end (Recepcionista) | `/api/v1/consultation/{patientId}/approve` | `PATCH` | Aprobar solicitud de contratación | Aprueba la consulta y cambia estado |
| 3 | MS1 - Clínica | Front-end (Nutricionista) | `/api/v1/patient/{patientId}/plan` | `POST` | Genera plan alimenticio | Asigna un plan alimenticio al paciente |
| 3.1 | **MS2 - Catálogo** | **MS1 - Clínica** | `/api/v1/catalog/plans/{id}` | `GET` | - Devuelve el plan | *[Auxiliar] MS1 consulta a MS2 para validar que el Plan ID existe antes de asignarlo* |
| 4 | MS1 - Clínica | Front-end (Nutricionista) | `/api/v1/patient/{planAlimenticioId}/schedulePlan` | `PATCH` | Programa próximo control evaluación | Agenda la próxima cita de evaluación |
| 5 | MS1 - Clínica | Front-end (Nutricionista) | `/api/v1/patient/{planAlimenticioId}/CreateControlEvaluation` | `POST` | Agregar una evaluación de control de un plan alimenticio | |
| 6 | MS1 - Clínica | Front-end (Nutricionista) | `/api/v1/patient/{planAlimenticioId}/controlEvaluationHistory` | `GET` | Historial de control Evaluaciones | Devuelve historial de evaluaciones |
| 7 | MS1 - Clínica | Front-end (Nutricionista) | `/api/v1/consultations/{patientId}/consultations` | `GET` | Historial de consultas del paciente | |
| 8 | MS1 - Clínica | Front-end (Paciente) | `/api/v1/patient/{id}/evolutions` | `GET` | Consultar seguimiento de la evolución de sus controles | Paciente visualiza su evolución de mediciones en la app |