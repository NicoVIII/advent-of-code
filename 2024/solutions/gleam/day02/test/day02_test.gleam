import gleam/int
import gleam/option.{Some}
import gleeunit
import testbase

import day02

pub fn testset_test() {
  let config =
    testbase.Config(
      day: "02",
      part1: Some(fn(input) {
        day02.parse(input) |> day02.part1 |> int.to_string
      }),
      part2: Some(fn(input) {
        day02.parse(input) |> day02.part2 |> int.to_string
      }),
    )
  testbase.testset_test(config)
}

pub fn main() {
  gleeunit.main()
}
