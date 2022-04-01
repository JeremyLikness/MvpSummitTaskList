# MvpSummitTaskList

This project was first shown at the 2022 Global MVP summit. It contains a Blazor Server and a Blazor WebAssembly project with shared UI using a Razor Class Library.

It is intended to demonstrate two features.

## Visual Studio Tooling for EF Core Migrations

Starting with [Visual Studio 2022 17.2 preview 2](https://visualstudio.microsoft.com/vs/preview/), you can create migrations and update your database without going to the command line! See [step-by-step: EF Core Migrations in Visual Studio 2022](./docs/migrations.md) for more.

🎬[Video: EF Core Migrations in Visual Studio 2022](https://youtu.be/j2XuiWq9Qes)

## SQLite (Persisted) in the Browser

[Steven Sanderson's](https://twitter.com/stevensanderson) [Blaze Orbital demo](https://github.com/SteveSandersonMS/BlazeOrbital) showcased SQLite in the browser. He custom built [a native WASM SQLite client](https://github.com/SteveSandersonMS/BlazeOrbital/blob/main/BlazeOrbital/ManufacturingHub/Data/e_sqlite3.o), modified the project template to include a [native file reference](https://github.com/SteveSandersonMS/BlazeOrbital/blob/main/BlazeOrbital/ManufacturingHub/BlazeOrbital.ManufacturingHub.csproj#HL30) and [used IndexDb with a timer](https://github.com/SteveSandersonMS/BlazeOrbital/blob/main/BlazeOrbital/ManufacturingHub/wwwroot/dbstorage.js) to save the database.

This demo takes advantage of [Erik Sink's](https://twitter.com/eric_sink) updates to his [SQLite PCL client](https://github.com/ericsink/SQLitePCL.raw) to easily add the WASM support, then uses a [custom factory](https://github.com/JeremyLikness/MvpSummitTaskList/blob/main/MvpSummitWasm/Data/SynchronizedSummitDbContextFactory.cs) and the [SavedChanges](https://github.com/JeremyLikness/MvpSummitTaskList/blob/main/MvpSummitWasm/Data/SynchronizedSummitDbContextFactory.cs#HL41) event to persist using browser [cache storage](https://developer.mozilla.org/en-US/docs/Web/API/Cache). See [step-by-step: SQLite with EF Core in the Browser](./docs/wasm.md) for more.

🎬[Video: EF Core and SQLite in Blazor Wasm](https://youtu.be/2UPiKgHv8YE)
