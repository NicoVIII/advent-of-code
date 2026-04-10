# README

To create a new day, run `gleam new day<day>`. After that delete the .github folder and the README.

Shared libraries live under `lib/`:
- `lib/solutionbase` — runtime helpers (`run` for main); add as a dependency: `solutionbase = { path = "../lib/solutionbase" }`
- `lib/testbase` — test harness; add as a dev dependency: `testbase = { path = "../lib/testbase" }`

If your puzzle needs any extra packages (e.g. `splitter`), add them with `gleam add <package>` from inside the day folder.

To start with your implementation of a day, you can start with the following template:

```gleam
import gleam/function
import solutionbase

pub fn main() -> Nil {
  solutionbase.run(function.identity, fn(_data) { "-" }, fn(_data) { "-" })
}
```

As a start for testing, you can add a test file with the following template:

```gleam
import gleam/function
import gleam/option.{Some}
import gleeunit

import day01
import testbase

pub fn testset_test() {
  testbase.run_for_testset(
    function.identity,
    option.None,
    option.None,
    day: "01",
  )
}

pub fn main() {
  gleeunit.main()
}
```
