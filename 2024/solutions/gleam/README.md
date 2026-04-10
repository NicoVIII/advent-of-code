# README

To create a new day, run `gleam new day<day>`. After that delete the .github folder and the README.

Shared libraries live under `lib/`:
- `lib/solutionbase` — runtime helpers (`run` for main); add as a dependency: `solutionbase = { path = "../lib/solutionbase" }`
- `lib/testbase` — test harness; add as a dev dependency: `testbase = { path = "../lib/testbase" }`

If your puzzle needs any extra packages (e.g. `splitter`), add them with `gleam add <package>` from inside the day folder.
