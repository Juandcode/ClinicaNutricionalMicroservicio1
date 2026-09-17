---
name: test-coverage
description: Corre y verifica los tests del proyecto GestionClinicaNutricionalService (unitarios en Tests/ e integración en IntegrationTests/), genera el reporte de cobertura con ReportGenerator y resume qué clases quedaron cubiertas, parcialmente cubiertas o pendientes. Úsalo cuando el usuario pida correr los tests, verificar los tests de integración, ver la cobertura actual, o actualizar el reporte/screenshot de cobertura del README.
---

# Test & Coverage

Este skill reproduce el flujo de testing y cobertura de este repo:

- `Tests` — tests unitarios (NUnit + FakeItEasy), cobertura con `coverlet.collector` + `ReportGenerator`.
- `IntegrationTests` — tests de integración (xUnit + `WebApplicationFactory`) contra una base de datos SQL Server real.

## 1. Correr los tests unitarios

Desde la raíz del repo:

```bash
dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

- `coverlet.runsettings` (en la raíz) excluye `GestionClinicaNutricional.Infrastructure.Migrations.*` del cálculo de cobertura — no lo quites salvo que el usuario lo pida explícitamente.
- Si `dotnet test` falla por errores de compilación, arreglalos antes de seguir — no interpretes cobertura de un build roto.
- El resultado queda en `Tests/TestResults/<guid>/coverage.cobertura.xml`. Si hay corridas viejas y querés un reporte limpio, borrá `Tests/TestResults` antes de correr.

## 2. Verificar los tests de integración

El proyecto `IntegrationTests` levanta la app real (`IntegrationTests.Setup.WebApplicationFactory`) y pega contra una base de datos SQL Server real.

```bash
dotnet test IntegrationTests/IntegrationTests.csproj
```

- Requiere una instancia local de SQL Server Express (`localhost\SQLEXPRESS`, servicio `MSSQL$SQLEXPRESS`) — la misma que usa el entorno de desarrollo (`Database=ClinicaNutricional`). Si no está disponible:
  - **No** saltees la verificación en silencio ni la marques como exitosa.
  - Avisá al usuario que los tests de integración no se pudieron correr y por qué.
- Tienen que pasar **todos**. Si alguno falla, reportá el nombre del test y el error antes de seguir con el reporte de cobertura.
- Los tests crean datos reales en `ClinicaNutricional`; usan datos únicos (Guid) para no pisarse entre corridas. No agregues datos fijos.

### (Opcional) Incluir la cobertura de integración

Para que el reporte también refleje lo cubierto por los tests de integración (por ejemplo los repositorios de `Infrastructure`, que solo se ejercitan acá), corré también ese proyecto con `--collect` y pasá los dos reportes a `reportgenerator`:

```bash
dotnet test IntegrationTests/IntegrationTests.csproj --collect:"XPlat Code Coverage"

reportgenerator "-reports:Tests/TestResults/*/coverage.cobertura.xml;IntegrationTests/TestResults/*/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html;TextSummary"
```

Aclarale al usuario que, si hacés esto, los porcentajes del reporte ya no son comparables con los de la sección de cobertura del README (que hoy son solo de los tests unitarios).

## 3. Generar el reporte de cobertura

```bash
# Si no está instalado (una sola vez):
dotnet tool install -g dotnet-reportgenerator-globaltool

rm -rf coveragereport
reportgenerator "-reports:Tests/TestResults/*/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html;TextSummary"
```

Usá `*/coverage.cobertura.xml` (una sola estrella), no `**`. Si en algún momento corrés `dotnet test` con `--logger trx` a la vez que `--collect`, VSTest genera una carpeta extra tipo `TestResults/<usuario>_<máquina>_<fecha>/In/<máquina>/coverage.cobertura.xml` con un segundo reporte parcial; `**` la recoge y contamina el merge (aparece "MultiReportParser" en vez de "CoberturaParser" en el summary — señal de que se coló ese archivo de más). Con `*/coverage.cobertura.xml` solo se toma la carpeta con GUID que genera coverlet.

Esto produce `coveragereport/index.html` y `coveragereport/Summary.txt`.

## 4. Resumir el resultado

Leé `coveragereport/Summary.txt` y reportá al usuario, en texto (no hace falta abrir el HTML salvo que se pida):

- Resultado de los tests unitarios y de integración (pasados/fallados).
- Line coverage y method coverage globales.
- Clases al 100% (cubiertas).
- Clases entre 1-99% (parcialmente cubiertas) con su porcentaje.
- Clases en 0% (pendientes), agrupadas por ensamblado (Application / Domain / Infrastructure / WebApi).

No trates un handler o controller como "cubierto" solo porque el archivo aparece en el reporte — confirmá que su % sea 100 antes de decir que está cubierto.

## 5. (Opcional) Actualizar el screenshot y la sección de cobertura del README

Solo si el usuario lo pide explícitamente (por ejemplo "actualiza el screenshot de cobertura" o "actualiza el README con la cobertura actual"):

1. Tomar el screenshot del summary con Chrome headless (ajustá la ruta del `.exe` de Chrome/Edge si difiere):

   ```bash
   "/c/Program Files/Google/Chrome/Application/chrome.exe" --headless --disable-gpu \
     --screenshot="$PWD/docs/coverage-summary.png" --window-size=900,560 --hide-scrollbars \
     --force-device-scale-factor=1 "file:///<ruta-absoluta-con-%20-por-espacios>/coveragereport/index.html"
   ```

   La ruta del HTML debe ser absoluta y con `%20` en vez de espacios. El screenshot final vive en `docs/coverage-summary.png` (referenciado desde `README.md`).

2. Actualizar en `README.md`, dentro de la sección `## Tests` → `### Estado actual de la cobertura`:
   - Los porcentajes de line/method coverage.
   - Las listas de clases cubiertas / parciales / pendientes, según el nuevo `Summary.txt`.

## 6. Recordatorios

- `Tests/TestResults/` y `coveragereport/` están en `.gitignore` — son artefactos regenerables, no los agregues a git salvo pedido explícito.
- Si se agregan handlers o controllers nuevos sin test, avisá cuáles quedaron sin cubrir en vez de asumir que están cubiertos.
- El pipeline de CI (`.github/workflows/tests.yml`) corre el flujo de los tests unitarios en cada push/PR a `main`; los tests de integración todavía **no** están en el pipeline (dependen de SQL Server local). Si cambiás el comando local, mantené el workflow en sync.
