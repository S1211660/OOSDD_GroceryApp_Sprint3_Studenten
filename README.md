# BoodschappenApp (GroceryApp)

Een applicatie voor het beheren van boodschappenlijsten.

## Functionaliteiten

- **UC1-UC3:** Tonen van boodschappenlijsten, boodschappenlijstitems en producten
- **UC4:** Kleur van boodschappenlijst aanpassen
- **UC5:** Producten toevoegen aan boodschappenlijst
- **UC6:** Inlogfunctionaliteit voor gebruikers
- **UC7:** Delen boodschappenlijst
- **UC8:** Zoeken producten
- **UC9:** Registratiescherm

## Project Structuur

```
├── Grocery.App/           # UI Layer (Views, ViewModels)
├── Grocery.Core/          # Business Logic Layer (Services, Models, Interfaces)
├── Grocery.Core.Data/     # Data Access Layer (Repositories)
```

## Development Workflow (GitFlow)

Deze applicatie gebruikt **GitFlow** als branching strategie voor ontwikkeling.

### Branch Structuur

- **`main`** - Productie-klare releases
- **`development`** - Ontwikkel branch
- **`feature/*`** - Nieuwe functionaliteiten
- **`hotfix/*`** - Kritieke fixes voor productie
- **`release/*`** - Release voorbereiding branches

### Commit Conventies

- **feat:** nieuwe functionaliteit
- **fix:** bug fix
- **docs:** documentatie wijzigingen
- **test:** test gerelateerde wijzigingen

### Release Proces

1. **Features:** Alle features voor release merged naar `development`
2. **Testing & Bug fixes:** Laatste fixes in development branch
3. **Release Branch:** `git checkout -b release/v1.1.0` van `development`
4. **Release:** Merge naar `main` en `development`, tag aanmaken

## Test Pipeline
- Automatische tests bij elke Pull Request
- GitHub Actions workflow in `.github/workflows/MAUI_unit_tests.yaml`
- Tests moeten slagen voordat merge mogelijk is

## Versies

- **Major:** Breaking changes (v2.0.0)
- **Minor:** Nieuwe functionaliteit (v1.1.0)
- **Patch:** Bug fixes (v1.0.1)

## Licentie

MIT License
