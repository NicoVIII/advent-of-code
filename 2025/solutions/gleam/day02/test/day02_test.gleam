import gleam/list
import gleam/option.{Some}
import gleeunit
import id_range
import testbase

import day02
import id

pub fn id_range_fold_test() {
  // Arrange
  let range = id_range.new_exn(id.new_exn(1), to: id.new_exn(5))
  let expected_ids = [1, 2, 3, 4, 5]

  // Act
  let ids =
    id_range.fold(range, from: [], with: fn(ids, id) { [id.value(id), ..ids] })
    |> list.reverse

  // Assert
  assert ids == expected_ids
}

pub fn is_invalid_id_for_part2_valid_test() {
  // Arrange
  let valid_ids = [
    123_456,
    1234,
    12_321,
    12_312,
  ]
  let expected_results = list.map(valid_ids, fn(raw_id) { #(raw_id, False) })

  // Act
  let valid_results =
    list.map(valid_ids, fn(raw_id) {
      #(raw_id, day02.is_invalid_id_for_part2(id.new_exn(raw_id)))
    })

  // Assert
  assert valid_results == expected_results
}

pub fn is_invalid_id_for_part2_invalid_test() {
  // Arrange
  let invalid_ids = [
    121_212,
    123_123,
    1212,
    111,
  ]
  let expected_results = list.map(invalid_ids, fn(raw_id) { #(raw_id, True) })

  // Act
  let invalid_results =
    list.map(invalid_ids, fn(raw_id) {
      #(raw_id, day02.is_invalid_id_for_part2(id.new_exn(raw_id)))
    })

  // Assert
  assert invalid_results == expected_results
}

pub fn testset_test() {
  testbase.run_for_testset(
    day02.parse,
    Some(day02.solve_part1),
    option.None,
    day: "02",
  )
}

pub fn main() {
  gleeunit.main()
}
