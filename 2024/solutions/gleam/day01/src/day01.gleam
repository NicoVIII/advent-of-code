import gleam/int
import gleam/list
import gleam/string
import solutionbase

type Data =
  #(List(Int), List(Int))

pub fn parse(input: String) -> Data {
  string.trim(input)
  |> string.split("\n")
  |> list.map(fn(line) {
    let assert [x, y] =
      string.trim(line)
      |> string.split("   ")
      |> list.map(fn(x) {
        let assert Ok(number) = int.parse(x)
        number
      })
    #(x, y)
  })
  |> list.unzip
}

pub fn solve_part1(data: Data) {
  let #(list1, list2) = data
  let list1_sorted = list.sort(list1, int.compare)
  let list2_sorted = list.sort(list2, int.compare)
  list.zip(list1_sorted, list2_sorted)
  |> list.map(fn(pair) {
    let #(x, y) = pair
    int.absolute_value(x - y)
  })
  |> list.fold(0, int.add)
  |> int.to_string
}

pub fn solve_part2(data: Data) {
  let #(list1, list2) = data
  list.map(list1, fn(el) { list.count(list2, fn(el2) { el == el2 }) * el })
  |> list.fold(0, int.add)
  |> int.to_string
}

pub fn main() {
  solutionbase.run(parse, solve_part1, solve_part2)
}
