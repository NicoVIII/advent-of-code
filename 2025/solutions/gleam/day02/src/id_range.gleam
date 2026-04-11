import gleam/int
import id

pub opaque type T {
  IdRange(from: id.T, to: id.T)
}

// Nicer type alias for unqualified usage/imports
pub type IdRange =
  T

pub fn new(from from: id.T, to to: id.T) -> Result(IdRange, Nil) {
  case id.value(from), id.value(to) {
    from, to if from > to -> Error(Nil)
    _, _ -> Ok(IdRange(from: from, to: to))
  }
}

pub fn new_exn(from from: id.T, to to: id.T) -> IdRange {
  case new(from, to) {
    Ok(range) -> range
    Error(Nil) -> panic as { "Invalid IdRange: from is bigger than to" }
  }
}

pub fn fold(
  over range: IdRange,
  from initial: acc,
  with fun: fn(acc, id.T) -> acc,
) -> acc {
  let IdRange(from: from, to: to) = range
  int.range(
    from: id.value(from),
    to: id.value(to) + 1,
    with: initial,
    run: fn(acc, value) { fun(acc, id.new_exn(value)) },
  )
}
