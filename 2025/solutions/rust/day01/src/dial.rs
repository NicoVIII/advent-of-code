use crate::clicks::Clicks;

const DIAL_SIZE: i32 = 100;

#[derive(Clone, Copy, Debug, PartialEq, Eq)]
pub struct Dial {
    position: i32,
}

impl Dial {
    pub fn new(position: i32) -> Result<Self, String> {
        if !(0..DIAL_SIZE).contains(&position) {
            return Err(format!("Invalid dial position given: {position}!"));
        }

        Ok(Self { position })
    }

    pub fn position(self) -> i32 {
        self.position
    }

    pub fn turn_left(self, clicks: Clicks) -> Self {
        self.turn(-clicks.value())
    }

    pub fn turn_right(self, clicks: Clicks) -> Self {
        self.turn(clicks.value())
    }

    pub fn turn_left_and_count_zero_passes(self, clicks: Clicks) -> (Self, i32) {
        self.turn_and_count_zero_passes(-clicks.value())
    }

    pub fn turn_right_and_count_zero_passes(self, clicks: Clicks) -> (Self, i32) {
        self.turn_and_count_zero_passes(clicks.value())
    }

    fn turn(self, change: i32) -> Self {
        let new_position = (self.position + change).rem_euclid(DIAL_SIZE);
        Self::new(new_position).expect("normalized dial position must stay in range")
    }

    fn turn_and_count_zero_passes(self, change: i32) -> (Self, i32) {
        let absolute_change = change.abs();
        let full_rounds = absolute_change / DIAL_SIZE;
        let normalized_change = change % DIAL_SIZE;

        let zero_passes = if self.position == 0 {
            full_rounds
        } else if self.position + normalized_change <= 0 || self.position + normalized_change > 99 {
            full_rounds + 1
        } else {
            full_rounds
        };

        (self.turn(normalized_change), zero_passes)
    }
}

#[cfg(test)]
mod tests {
    use super::Dial;
    use crate::clicks::Clicks;

    fn clicks(value: i32) -> Clicks {
        Clicks::new(value).unwrap()
    }

    #[test]
    fn wraps_in_both_directions() {
        let dial = Dial::new(2).unwrap();

        assert_eq!(dial.turn_left(clicks(5)).position(), 97);
        assert_eq!(dial.turn_right(clicks(103)).position(), 5);
    }

    #[test]
    fn turn_left_and_count_zero_passes_matches_expected_cases() {
        let dial = Dial::new(50).unwrap();

        let (new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(clicks(25));
        assert_eq!(new_dial.position(), 25);
        assert_eq!(zero_passes, 0);

        let (new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(clicks(125));
        assert_eq!(new_dial.position(), 25);
        assert_eq!(zero_passes, 1);

        let (new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(clicks(75));
        assert_eq!(new_dial.position(), 75);
        assert_eq!(zero_passes, 1);

        let (new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(clicks(175));
        assert_eq!(new_dial.position(), 75);
        assert_eq!(zero_passes, 2);

        let (new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(clicks(50));
        assert_eq!(new_dial.position(), 0);
        assert_eq!(zero_passes, 1);

        let (new_dial, zero_passes) = dial.turn_left_and_count_zero_passes(clicks(150));
        assert_eq!(new_dial.position(), 0);
        assert_eq!(zero_passes, 2);

        let zero_dial = Dial::new(0).unwrap();
        let (new_dial, zero_passes) = zero_dial.turn_left_and_count_zero_passes(clicks(50));
        assert_eq!(new_dial.position(), 50);
        assert_eq!(zero_passes, 0);
    }

    #[test]
    fn turn_right_and_count_zero_passes_matches_expected_cases() {
        let dial = Dial::new(50).unwrap();

        let (new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(clicks(25));
        assert_eq!(new_dial.position(), 75);
        assert_eq!(zero_passes, 0);

        let (new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(clicks(125));
        assert_eq!(new_dial.position(), 75);
        assert_eq!(zero_passes, 1);

        let (new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(clicks(75));
        assert_eq!(new_dial.position(), 25);
        assert_eq!(zero_passes, 1);

        let (new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(clicks(175));
        assert_eq!(new_dial.position(), 25);
        assert_eq!(zero_passes, 2);

        let (new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(clicks(50));
        assert_eq!(new_dial.position(), 0);
        assert_eq!(zero_passes, 1);

        let (new_dial, zero_passes) = dial.turn_right_and_count_zero_passes(clicks(150));
        assert_eq!(new_dial.position(), 0);
        assert_eq!(zero_passes, 2);

        let zero_dial = Dial::new(0).unwrap();
        let (new_dial, zero_passes) = zero_dial.turn_right_and_count_zero_passes(clicks(50));
        assert_eq!(new_dial.position(), 50);
        assert_eq!(zero_passes, 0);
    }
}