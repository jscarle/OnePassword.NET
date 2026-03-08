# Guidance for AI contributors

This repository contains `OnePassword.NET`, a .NET wrapper for the 1Password CLI.

## Git Workflow

- When the user asks to commit changes, first check the current branch in the owning repository.
- If the current branch is not already a feature branch named in the format `feature/<appropriate-short-name>`, create one before committing.
- Choose a short, specific branch suffix that describes the work. Keep it lowercase and hyphenated.
- If already on an appropriate `feature/...` branch, do not create an additional branch unless the user asks for one.
- Commit messages must be a single-line short sentence in past tense that summarizes the commit.
- Commit messages must be written as a proper sentence and must end with a period.
- Do not use multiline commit messages, bullet lists, prefixes, or issue numbers in the commit message unless the user explicitly asks for them.
- After creating the commit, push the branch and its commits to `origin` unless the user explicitly says not to push.
- When the user says a pull request was merged, switch the owning repository back to `develop`, pull the latest changes from `origin`, prune deleted remote refs, and remove any local branches that no longer exist at `origin`.

## GitHub CLI

- Use the GitHub CLI (`gh`) for GitHub-related operations whenever possible.
- Prefer `gh` for pull request workflows, including creating, viewing, checking out, and reviewing pull requests.
- If `gh` is unavailable or unauthenticated, note that clearly before falling back to another approach.
