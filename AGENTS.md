# Guidance for AI contributors

This repository contains `OnePassword.NET`, a .NET wrapper for the 1Password CLI.

## Git Workflow

- When the user asks to commit changes, first check the current branch in the owning repository.
- If the current branch is not already a feature branch named in the format `feature/<appropriate-short-name>`, create one before committing.
- Choose a short, specific branch suffix that describes the work. Keep it lowercase and hyphenated.
- If already on an appropriate `feature/...` branch, do not create an additional branch unless the user asks for one.
- Do not leave your own repository changes uncommitted; commit them before ending the work unless the user explicitly asks you not to commit.
- When creating a commit, include all current unstaged changes in that repository in the commit unless the user explicitly asks to exclude something.
- Commit messages must be a single-line short sentence in past tense that summarizes the commit.
- Commit messages must be written as a proper sentence and must end with a period.
- Do not use multiline commit messages, bullet lists, prefixes, or issue numbers in the commit message unless the user explicitly asks for them.
- After creating the commit, push the branch and its commits to `origin` unless the user explicitly says not to push.
- When the user says a pull request was merged, switch the owning repository back to the default branch, which is currently `develop` in this repository, pull the latest changes from `origin`, prune deleted remote refs, and remove any local branches that no longer exist at `origin`.

## GitHub CLI

- Use the GitHub CLI (`gh`) for GitHub-related operations whenever possible.
- Prefer `gh` for pull request workflows, including creating, viewing, checking out, and reviewing pull requests.
- If `gh` is unavailable or unauthenticated, note that clearly before falling back to another approach.

## Generated Assets

- Do not read, search, or summarize generated documentation/site assets unless the user explicitly asks for them.
- In particular, avoid generated docfx output and bundled vendor assets such as minified JavaScript, CSS, or copied third-party files; prefer the markdown and source files under `docfx/` instead.

## API Abstraction

- Never expose or leak raw 1Password CLI responses through the public API unless the user explicitly asks for that exact behavior.
- Keep the wrapper abstraction stable and consumer-focused: parse CLI output into library models and shield consumers from CLI output-shape changes whenever practical.
