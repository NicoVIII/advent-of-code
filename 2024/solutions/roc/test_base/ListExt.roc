module [
    map!,
]

map! = \list, mapper! ->
    List.walk! list [] (\acc, el -> List.append acc (mapper! el))

expect map! [1, 2, 3] (\x -> x * 2) == [2, 4, 6]
