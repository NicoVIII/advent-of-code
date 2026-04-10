import gleam/bool
import gleam/dict.{type Dict}
import gleam/int
import gleam/list
import gleam/string
import solutionbase

type DataKey =
  #(Int, Int)

type DataValue =
  String

pub type Data {
  Data(dict: Dict(DataKey, DataValue), x_max: Int, y_max: Int)
}

pub fn parse(input: String) -> Data {
  let dict_list =
    input
    |> string.trim()
    |> string.split("\n")
    |> list.map_fold(0, fn(idx1, line) {
      let list =
        string.to_graphemes(line)
        |> list.map_fold(0, fn(idx2, char) {
          #(idx2 + 1, #(#(idx2, idx1), char))
        })
        |> fn(x) { x.1 }
      #(idx1 + 1, list)
    })
    |> fn(x) { x.1 }
    |> list.flatten()
  let assert Ok(#(#(x_max, _), _)) =
    list.max(dict_list, fn(e1, e2) {
      let #(#(x1, _), _) = e1
      let #(#(x2, _), _) = e2
      int.compare(x1, x2)
    })
  let assert Ok(#(#(_, y_max), _)) =
    list.max(dict_list, fn(e1, e2) {
      let #(#(_, y1), _) = e1
      let #(#(_, y2), _) = e2
      int.compare(y1, y2)
    })
  Data(dict.from_list(dict_list), x_max, y_max)
}

pub fn solve_part1(data: Data) -> String {
  let check = fn(x_key: DataKey, next_key: fn(DataKey) -> DataKey) -> Bool {
    let get = fn(key) {
      let assert Ok(value) = dict.get(data.dict, key)
      value
    }

    use <- bool.guard(get(x_key) != "X", False)
    let m_key = next_key(x_key)
    use <- bool.guard(get(m_key) != "M", False)
    let a_key = next_key(m_key)
    use <- bool.guard(get(a_key) != "A", False)
    let s_key = next_key(a_key)
    get(s_key) == "S"
  }

  let check_left = fn(key) {
    let #(x, _) = key
    use <- bool.guard(x < 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x - 1, y)
    })
  }

  let check_up = fn(key) {
    let #(_, y) = key
    use <- bool.guard(y < 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x, y - 1)
    })
  }

  let check_right = fn(key) {
    let #(x, _) = key
    use <- bool.guard(x > data.x_max - 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x + 1, y)
    })
  }

  let check_down = fn(key) {
    let #(_, y) = key
    use <- bool.guard(y > data.y_max - 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x, y + 1)
    })
  }

  let check_up_left = fn(key) {
    let #(x, y) = key
    use <- bool.guard(x < 3, False)
    use <- bool.guard(y < 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x - 1, y - 1)
    })
  }

  let check_up_right = fn(key) {
    let #(x, y) = key
    use <- bool.guard(x > data.x_max - 3, False)
    use <- bool.guard(y < 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x + 1, y - 1)
    })
  }

  let check_down_left = fn(key) {
    let #(x, y) = key
    use <- bool.guard(x < 3, False)
    use <- bool.guard(y > data.y_max - 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x - 1, y + 1)
    })
  }

  let check_down_right = fn(key) {
    let #(x, y) = key
    use <- bool.guard(x > data.x_max - 3, False)
    use <- bool.guard(y > data.y_max - 3, False)
    check(key, fn(key) {
      let #(x, y) = key
      #(x + 1, y + 1)
    })
  }

  data.dict
  |> dict.to_list()
  |> list.fold(0, fn(sum, entry) {
    let #(key, _) = entry
    [
      check_left,
      check_up,
      check_right,
      check_down,
      check_up_left,
      check_up_right,
      check_down_left,
      check_down_right,
    ]
    |> list.count(fn(check_fn) { check_fn(key) })
    |> fn(count) { sum + count }
  })
  |> int.to_string
}

pub fn solve_part2(data: Data) -> String {
  let get = fn(key) {
    let assert Ok(value) = dict.get(data.dict, key)
    value
  }

  data.dict
  |> dict.to_list()
  |> list.count(fn(entry) {
    let #(key, _) = entry
    let #(x, y) = key
    use <- bool.guard(x == 0, False)
    use <- bool.guard(y == 0, False)
    use <- bool.guard(x == data.x_max, False)
    use <- bool.guard(y == data.y_max, False)
    use <- bool.guard(get(key) != "A", False)
    let top_left = get(#(x - 1, y - 1))
    let top_right = get(#(x + 1, y - 1))
    let bottom_left = get(#(x - 1, y + 1))
    let bottom_right = get(#(x + 1, y + 1))

    let diag1 =
      top_left == "M"
      && bottom_right == "S"
      || top_left == "S"
      && bottom_right == "M"
    let diag2 =
      top_right == "M"
      && bottom_left == "S"
      || top_right == "S"
      && bottom_left == "M"
    diag1 && diag2
  })
  |> int.to_string
}

pub fn main() {
  solutionbase.run(parse, solve_part1, solve_part2)
}
