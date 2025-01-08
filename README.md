# SmartCredit
![](demo.gif)

**PRUEBA LA DEMO EN AZURE**: 
https://smartcreditwebapp-aaascnf2fre7ctg7.canadacentral-01.azurewebsites.net/
---


## Índice
1. [Descripción](#descripción)
2. [Funcionalidades principales](#funcionalidades-principales)
3. [Tecnologías utilizadas](#tecnologías-utilizadas)
   - [Backend](#backend)
   - [Frontend](#frontend)
   - [Base de datos](#base-de-datos)
4. [Estructura del proyecto](#estructura-del-proyecto)
5. [Configuración del entorno](#configuración-del-entorno)
   - [Requisitos previos](#requisitos-previos)
   - [Pasos para ejecutar el proyecto](#pasos-para-ejecutar-el-proyecto)
6. [Endpoints principales](#endpoints-principales)
   - [CreditCard](#creditcard)
   - [Transactions](#transactions)
7. [Extras implementados](#extras-implementados)
8. [Licencia](#licencia)

## Descripción
Este proyecto es una aplicación web para gestionar el estado de cuenta de una tarjeta de crédito. Permite visualizar movimientos, realizar pagos y exportar datos en formatos como PDF.

El sistema se desarrolló utilizando tecnologías .NET 6, ASP.NET Web API, Razor, jQuery y SQL Server, cumpliendo con las mejores prácticas de desarrollo y principios SOLID.

---

## Funcionalidades principales
- **Pantalla de Estado de Cuenta:**
  - Visualización del saldo actual, límite de crédito y saldo disponible.
  - Detalle de las compras realizadas en el mes actual.
  - Cálculo de intereses y cuotas mínimas.
  - Exportación del estado de cuenta en formato PDF.

- **Pantalla de Compras:**
  - Formulario para agregar nuevas compras.

- **Pantalla de Pagos:**
  - Formulario para registrar pagos realizados.

- **Historial de Transacciones:**
  - Listado de todas las transacciones realizadas en el mes actual, ordenadas por fecha descendente.

---

## Tecnologías utilizadas

### Backend
- **ASP.NET Web API**: Implementación de endpoints RESTful.
- **Entity Framework Core**: Manejo de datos e interacción con SQL Server.
- **FluentValidation**: Validaciones centralizadas.
- **AutoMapper**: Mapeo de DTOs y modelos de dominio.
- **CQRS**: Separación de comandos y consultas.
- **Swagger**: Documentación interactiva de la API.
- **HealthChecks**: Monitoreo del estado del sistema.

  ![image](https://github.com/user-attachments/assets/d7474cdf-a195-4db8-9c85-ecc2cef4d834)


### Frontend
- **Razor**: Renderizado de vistas dinámicas.
- **jQuery**: Manipulación de DOM y peticiones AJAX.
- **FluentValidation**: Validaciones centralizadas.
- **EPPlus**: Generación de reportes.
- **Bootstrap 5**: Librería de estilos.

### Base de datos
- **SQL Server**: Almacenamiento de información con procedimientos almacenados (PL/SQL).

---

## Estructura del proyecto
```
src/
├───BackEnd/
│   ├───Core/
│   │   ├───SmartCredit.BackEnd.Application
│   │   ├───SmartCredit.BackEnd.Domain
│   ├───Infrastructure/
│   │   ├───SmartCredit.BackEnd.Persistence
│   └───Presentation/
│       ├───SmartCredit.BackEnd.WebApi
├───FrontEnd/
    └───Presentation/
        └───SmartCredit.FrontEnd.WebApp
```
### Explicación de la Arquitectura

El proyecto sigue un enfoque de **Clean Architecture**, que separa las responsabilidades en capas bien definidas para mantener la escalabilidad, mantenibilidad y testabilidad del código. A continuación, se describen las principales características implementadas:

1. **Clean Architecture**:
   - La solución está dividida en capas: 
     - **Core**: Contiene la lógica de negocio y reglas de dominio. Incluye:
       - **Domain**: Define las entidades y abstracciones principales.
       - **Application**: Contiene casos de uso, DTOs, validaciones y lógica específica.
     - **Infrastructure**: Proporciona la implementación de acceso a datos y servicios externos, incluyendo Entity Framework para manejar la persistencia.
     - **Presentation**: Implementa la API REST para interactuar con el sistema, siguiendo los principios de la arquitectura orientada a servicios.
   - Cada capa interactúa solo con las capas más cercanas, manteniendo las dependencias dirigidas hacia el núcleo (Core).

2. **CQRS (Command Query Responsibility Segregation)**:
   - La separación de comandos (modificación del estado) y consultas (lectura de datos) mejora la claridad y la eficiencia del sistema.
   - Los comandos y consultas están organizados en la capa **Application** dentro de carpetas como `Features/CreditCards` y `Features/Transactions`.
   - Cada comando y consulta utiliza patrones como `Handlers` para implementar su lógica.

3. **Unit of Work**:
   - Se utiliza el patrón Unit of Work para garantizar la consistencia en las transacciones de base de datos.
   - La capa de persistencia gestiona todas las operaciones de repositorio como una única unidad, minimizando los errores y simplificando el manejo de transacciones.

Esta arquitectura asegura una alta cohesión dentro de las capas y un bajo acoplamiento entre ellas, permitiendo una evolución del sistema sin afectar el núcleo.


---

## Configuración del entorno

### Requisitos previos
- **.NET 6** ([SDK v6.0.428](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.428-windows-x64-installer "SDK v6.0.428"))
- **SQL Server 2019**
- **Visual Studio 2022** (opcional)

### Pasos para ejecutar el proyecto
1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/juanerroa/smart-credit.git
   ```
2. **Configurar la base de datos:**

   - Asegurarse que la base de datos no exista
   
   - Agregar cadena de conexión  de la base de datos a ser:
   `src\BackEnd\Presentation\SmartCredit.BackEnd.WebApi\appsettings.json`.
   
 - No es necesario aplicar las migraciones, el proyecto las aplicara de manera automatica al ser ejecutado en caso la base de datos no exista.
 
3. **Ejecutar el Backend:**
   ```bash
   cd src/BackEnd/Presentation/SmartCredit.BackEnd.WebApi
   dotnet run
   ```

4. **Ejecutar el Frontend:**
   ```bash
   cd src/FrontEnd/Presentation/SmartCredit.FrontEnd.WebApp
   dotnet run
   ```

5. **Acceso a la aplicación:**
   - Abrir el navegador e ir a `http://localhost:6001`.

---

## Endpoints principales
### CreditCard
- **GET /api/CreditCard/GetAll**: Obtiene todas las tarjetas de crédito registradas.
- **GET /api/CreditCard/GetById/{Id}**: Obtiene una tarjeta de crédito específica mediante su ID.
- **POST /api/CreditCard/AddUserAndCreditCard**: Registra un usuario y asocia una tarjeta de crédito.
- **POST /api/CreditCard/GetCreditCardStatement**: Obtiene el estado de cuenta de una tarjeta de crédito.

### Transactions
- **POST /api/Transactions/GetByPeriod**: Obtiene transacciones realizadas en un período específico.
- **POST /api/Transactions/AddPurchase**: Registra una nueva compra.
- **POST /api/Transactions/AddPayment**: Registra un nuevo pago.

---

## Extras implementados
- **Exportación de compras a Excel**..
- **Despliegue en Azure con acceso público**

---

## Documentación extra
- **Documentación de la API**..
  [Click aquí](https://github.com/juanerroa/smart-credit/blob/main/Documentaci%C3%B3n_API%20.pdf
)
- **Colección POSTMAN**..
  [Click aquí](https://github.com/juanerroa/smart-credit/blob/main/POSTMAN_COLLECTION.json)
---

## Licencia
Este proyecto está licenciado bajo [Licencia MIT](LICENSE).

