use std::str::FromStr;

#[derive(Clone, Copy, Debug, PartialEq, Eq)]
pub struct Clicks(i32);

impl Clicks {
    pub fn new(value: i32) -> Result<Self, String> {
        if value <= 0 {
            return Err(format!("Invalid clicks value given: {value}!"));
        }

        Ok(Self(value))
    }

    pub fn value(self) -> i32 {
        self.0
    }
}

impl FromStr for Clicks {
    type Err = String;

    fn from_str(value: &str) -> Result<Self, Self::Err> {
        let parsed = value
            .parse::<i32>()
            .map_err(|_| format!("Invalid clicks value given: {value}!"))?;

        Self::new(parsed)
    }
}

#[cfg(test)]
mod tests {
    use super::Clicks;

    #[test]
    fn rejects_non_positive_values() {
        assert_eq!(Clicks::new(0).unwrap_err(), "Invalid clicks value given: 0!");
        assert_eq!(Clicks::new(-1).unwrap_err(), "Invalid clicks value given: -1!");
    }

    #[test]
    fn parses_positive_values() {
        assert_eq!("103".parse::<Clicks>().unwrap().value(), 103);
    }
}