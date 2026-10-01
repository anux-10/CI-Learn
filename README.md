# CI-Learn

Learning Continuous Integration with GitHub Actions.

This repo is for practicing CI: every Pull Request to `main` automatically builds, tests, and lints the code before merging.

## How it works

`Pull Request to main` -> `CI Workflow` -> `Jobs on Runners`

1. **test** - `dotnet restore`, `dotnet build`, `dotnet test`
2. **lint** - `dotnet format --verify-no-changes`
3. **deploy** - needs `test` + `lint` to pass, then publishes to `staging` environment

Runners use `actions/checkout@v4` to get the code, then `actions/setup-dotnet@v4` to build it.

## Repo structure
