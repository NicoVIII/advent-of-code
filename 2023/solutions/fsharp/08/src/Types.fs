namespace Day08

type Node = string

type Instruction =
    | Left
    | Right

module Instruction =
    let fromValue =
        function
        | 'L' -> Left
        | 'R' -> Right
        | value -> failwith $"Invalid Input: '{value}'"

type NodeMap = Map<Node, Node * Node>
