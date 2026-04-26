# 🖨️ 3DPrintFlow – Gestor de Órdenes para Servicio de Impresiones 3D

![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![C#](https://img.shields.io/badge/C%23-13-blue)
![EF Core](https://img.shields.io/badge/EF_Core-9-orange)
![License](https://img.shields.io/badge/License-MIT-green)

Sistema profesional de gestión de órdenes de impresión 3D. Permite administrar clientes, materiales, impresoras y órdenes con un flujo completo de estados, cálculo automático de costos, reportes y autenticación por roles (Admin / Operador).

**Arquitectura:** API REST con capas separadas (Domain → Application → Infrastructure → Persistence → API) + cliente MVC independiente. Comunicación entre proyectos vía HTTP/JSON.

---

## ✨ Características principales

- ✅ **CRUD completo** de Clientes, Materiales, Impresoras y Órdenes
- ✅ **Reglas de negocio** reales: control de stock, flujo de estados, motivo obligatorio de cancelación, horas de uso por impresora
- ✅ **Cálculo automático** de tiempo y costo estimado con sobrecarga de métodos
- ✅ **Asignación automática de impresora** según tecnología, disponibilidad y velocidad
- ✅ **Búsqueda avanzada** de órdenes por cliente, fecha, estado y material
- ✅ **Historial de estados** con fechas automáticas de inicio, fin y entrega
- ✅ **Comprobante de entrega** descargable en `.txt`
- ✅ **Reportes con exportación CSV:**
  - Materiales más usados
  - Ingresos proyectados vs reales (con filtro de fechas)
  - Tiempo total de uso por impresora
- ✅ **Autenticación con roles** Admin / Operador usando ASP.NET Core Identity
- ✅ **Dashboard** con estadísticas en tiempo real y últimas órdenes
- ✅ **Diseño moderno** con sidebar, tarjetas y badges de estado
- ✅ **Swagger UI** disponible en `/swagger` para explorar la API

---

## 🏗️ Arquitectura del proyecto

```
Impresiones3D.sln
│
├── Impresiones3D.Domain          → Entidades, interfaces, enums (sin dependencias)
├── Impresiones3D.Application     → Servicios, DTOs, Requests
├── Impresiones3D.Infrastructure  → Repositorios, UnitOfWork
├── Impresiones3D.Persistence     → DbContext, configuraciones EF, migraciones
├── Impresiones3D.Api             → API REST (controllers, Swagger, CORS)
└── Impresiones3D.Web             → Cliente MVC (vistas, Identity, ApiClient)
```

---

## 🛠️ Tecnologías utilizadas

| Proyecto | Tecnologías |
|---|---|
| **API** | .NET 9, ASP.NET Core, EF Core 9, SQL Server, Swagger, CORS |
| **Web (MVC)** | .NET 9, ASP.NET Core MVC, Bootstrap 5, Bootstrap Icons, Identity |
| **Base de datos** | SQL Server / LocalDB |
| **Patrones** | Repository, Unit of Work, Clean Architecture, Dependency Injection |

---

## 🚀 Instrucciones para ejecutar el proyecto localmente

### Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express, Developer o LocalDB)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o VS Code

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/Impresiones3D.git
cd Impresiones3D
```

### 2. Configurar la cadena de conexión

Edita el archivo `appsettings.json` en **ambos** proyectos:

**`Impresiones3D.Api/appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Impresiones3DDb;Trusted_Connection=True;Encrypt=false"
  }
}
```

**`Impresiones3D.Web/appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Impresiones3DIdentityDb;Trusted_Connection=True;Encrypt=false"
  },
  "ApiSettings": {
    "BaseUrl": "https://localhost:7130"
  }
}
```

> Si usas LocalDB, reemplaza `Server=localhost` por `Server=(localdb)\\mssqllocaldb`.

### 3. Aplicar las migraciones

Abre una terminal en la raíz del proyecto y ejecuta:

```bash
# Migración de la base de datos principal (órdenes, materiales, etc.)
dotnet ef database update --project Impresiones3D.Persistence --startup-project Impresiones3D.Api

# Migración de Identity (usuarios y roles)
dotnet ef database update --project Impresiones3D.Web --startup-project Impresiones3D.Web
```

> Las bases de datos se crean automáticamente si no existen.

### 4. Ejecutar los proyectos

Necesitas correr **los dos proyectos al mismo tiempo**.

**Opción A — Visual Studio:**
1. Click derecho en la solución → *Set Startup Projects*
2. Selecciona *Multiple startup projects*
3. Marca `Impresiones3D.Api` y `Impresiones3D.Web` como **Start**
4. Presiona `F5`

**Opción B — Terminal (dos ventanas):**

```bash
# Terminal 1 — API
cd Impresiones3D.Api
dotnet run

# Terminal 2 — Web
cd Impresiones3D.Web
dotnet run
```

### 5. Acceder a la aplicación

| Servicio | URL |
|---|---|
| **Aplicación Web** | https://localhost:7208 |
| **API REST** | https://localhost:7130 |
| **Swagger UI** | https://localhost:7130/swagger |

---

## 👥 Credenciales de prueba

| Rol | Correo | Contraseña |
|---|---|---|
| Administrador | admin@3dprintflow.com | Admin123! |
| Operador | operador@3dprintflow.com | Operador123! |

> Las cuentas se crean automáticamente al iniciar la aplicación por primera vez (`DbInitializer`).

---

## 📋 Conceptos de POO implementados

| Concepto | Dónde se aplica |
|---|---|
| **Clase abstracta** | `PersonaBase` — base de `Cliente` |
| **Herencia** | `Cliente : PersonaBase` |
| **Polimorfismo** | `override ObtenerRol()` en `Cliente` |
| **Sobrecarga de métodos** | `CalcularEstimados()` y `CalcularEstimados(decimal costoPorHora)` en `Orden` |
| **Encapsulamiento** | Propiedades con `{ get; set; }`, lógica de negocio en servicios |
| **Interfaces** | `IBaseRepository<T>`, `IUnitOfWork`, interfaces por repositorio |

---

## 📄 Licencia

Este proyecto está bajo la licencia [MIT](LICENSE).
