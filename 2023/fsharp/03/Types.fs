namespace Day03

type EngineSchematic = string[]
type SymbolPosition = { row: int; col: int }
type Symbol = { text: char; position: SymbolPosition }
type EngineStructure = (uint * Symbol) list

type Gear = uint * uint
