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

## Tests

- In this repository, skipped live tests usually mean the configured test account does not support the required operations for those scenarios.
- When reporting test results, call out that capability limitation explicitly instead of treating those skips as product failures by default.

## API Abstraction

- Never expose or leak raw 1Password CLI responses through the public API unless the user explicitly asks for that exact behavior.
- Keep the wrapper abstraction stable and consumer-focused: parse CLI output into library models and shield consumers from CLI output-shape changes whenever practical.

## Release Intent, Versioning, and Workflow

Treat explicit release requests—such as “Mint a new release,” “Release this,” “Publish this as a new release,” “Create a new release,” or any clear equivalent—as authorization to carry out OnePassword.NET's full release workflow, including committing, pushing, tagging, and publishing as needed.

If the user has not clearly requested a release, or if the appropriate change category cannot be determined confidently, ask for clarification **before** committing, pushing, creating a tag, or publishing a release.

Only change OnePassword.NET's package, assembly, and file versions when the user has clearly requested that a release be published. Do not bump versions during ordinary implementation, refactoring, bug-fixing, dependency updates, testing, commits, pushes, or pull-request preparation unless those actions are part of an explicitly requested release.

Choose the next version using these repository-specific rules:

- Minor, backward-compatible changes increment the patch component. Example: `2.5.0` → `2.5.1`.
- Moderate or breaking changes increment the middle component and reset the patch. Example: `2.5.0` → `2.6.0`.
- Increment the major component and reset the other components only when the newest supported .NET generation changes. Example: `2.5.0` → `3.0.0`.
- Breaking changes alone do not justify a major-version increment in this repository. If multiple categories apply, use the highest applicable category.

Update `Version` in `OnePassword.NET/OnePassword.NET.csproj` using three components and update `AssemblyVersion` and `FileVersion` using the matching four-component form. Use a three-component lightweight tag such as `v2.5.1`; OnePassword.NET's release tags are lightweight and must remain so.

A clear release request authorizes this complete workflow:

1. Inspect the worktree, current branch, `origin`, `develop`, current CLI compatibility, recent commits, pull requests, issues, contributors, and releases before editing release metadata.
2. Determine the next version, update the three version properties in `OnePassword.NET/OnePassword.NET.csproj`, and verify that the tag and release do not already exist. Use `NEXT_RELEASE.md` as release-note input when it exists.
3. Validate the release-scoped changes using the repository's normal Release build and test commands, clearly reporting capability-limited skipped live tests.
4. Create or reuse a `feature/release-X-Y-Z` branch for release preparation, following the existing `feature/<short-name>` branch policy.
5. Commit all release-scoped changes using a single-line past-tense sentence ending in a period, then push the release branch.
6. Open a ready pull request against `develop` titled `Prepare the X.Y.Z release`, with `Summary` and `Testing` sections consistent with recent release pull requests.
7. Enable auto-merge using a merge commit, not squash, to preserve OnePassword.NET's current pull-request history convention. Monitor checks and review state, fix and push blockers, and wait until the pull request is merged.
8. Fast-forward local `develop` from `origin/develop`, prune deleted remote refs, and remove the merged local release branch when appropriate.
9. Create the lightweight `vX.Y.Z` tag on the latest `develop` commit and push that tag to `origin`.
10. Publish a non-draft GitHub release named `vX.Y.Z - Description`. Follow recent structured notes when applicable, including Compatibility, Breaking changes, Changes, Contributors, and Thanks sections without inventing empty sections.
11. Monitor the release-triggered `Publish` workflow until the OnePassword.NET NuGet package is published successfully, fixing in-scope failures when possible.

Do not require separate confirmation for each Git or GitHub step after an unambiguous release command. Report the pull request, merge commit, tag, GitHub release URL, and NuGet publishing result when complete.
