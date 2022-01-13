# API RestFull Basic RepositoryPattern

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Es un proyecto orientado a la creacion de una estructura base para proyecto desarrollo de API RestFull con ASP.Net Core Web Project (API).
# New Features!

  - 05/09/2020 Creacion proyecto base con flujo de usuarios.

### Tech

El proyecto usa una serie de paquetes nuggets para funcionar correctamente:

* [AutoMapper.Extensions.Microsoft.DependencyInjection] - Mapeo entre objetos
* [Microsoft.EntityFrameworkCore] - ORM para el manejo de Transact-SQL
* [NLog.Extensions.Logging] - Para el servicio de Logs
* [Pomelo.EntityFrameworkCore.MySql] - Para la conexion con MySql

asi como tambien el presente proyecto es un [proyecto publico] de GitHub.

### Installation

Para instalar las dependencias del proyecto se puede ejecutar los siguientes comandos:

```sh
PM> Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection -Version 8.0.1
PM> Install-Package Microsoft.EntityFrameworkCore -Version 3.1.7
PM> Install-Package NLog.Extensions.Logging -Version 1.6.5
PM> Install-Package Pomelo.EntityFrameworkCore.MySql -Version 3.1.2
```

Para la recreacion de la BD en el proyecto "Entities"...

```sh
PM> Add-Migration DbInit
PM> Update-Database
```

### Development

Quieres contribuir? Genial!
puedes agregarme a tu red de [LinkedIn] o enviarme un correo a apugliesisoliz@gmail.com para solicitar el pull request.


### To-do

 - Incluir Pryecto de Pruebas Unitarias para implementar TDD
 - Incluir Seguridad con Bearer Token
 - Inluir Swagger para documentacion de API


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)



   [proyecto publico]: <https://github.com/apugliesisoliz/APIRestFull_Basic_RepositoryPattern>
   [AutoMapper.Extensions.Microsoft.DependencyInjection]: <https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/>
   [Microsoft.EntityFrameworkCore]: <https://www.nuget.org/packages/Microsoft.EntityFrameworkCore>
   [NLog.Extensions.Logging]: <https://www.nuget.org/packages/NLog.Extensions.Logging/>
   [Pomelo.EntityFrameworkCore.MySql]: <https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql>
   [LinkedIn]: <https://www.linkedin.com/in/alejandropugliesi/>
