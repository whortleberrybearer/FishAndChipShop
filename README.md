# The Fish and Chip Shop Problem
Scenario tests follow a light BDD style due to well defined requirments.  SpecFlow seemed overkill for the scope of the task.

## Part 1
A customer can buy a portion of chips.
- A portion of chips costs £1.80

## Part 2
A customer can buy a pie.
- A pie costs £3.20
- A pie has an expiry date
- A pie cannot be sold if it is past the expiry date
- A pie is sold at a discounted rate of 50% on the day of expiry only

## Part 3
A customer can buy a pie and chips meal deal.
- A pie and chips meal deal applies a 20% discount to both items
- The discount can be applied to multiple meal deals
- The discount is not applied to items outside of a meal deal (for example, if there are 2 pies and 3 portions of chips in the basket, only 2 pies and 2 portions of chips should be discounted)
- Items in a meal deal are not eligible for any other discounts
- When multiple discounts may be applied, then the customer should always be offered the lowest total price


Additions
- Defected pie discount
- Sell fish / types of fish
- Time / day based discounts
- You can only buy items in stock
- Date based stock management
- Reporting
- Customer loyalty point / discount
- New customer discount
- Allergens / Vegetarian / Vegan
- External integrations
- Big chips / small chips (amount based on weight, e.g. have enough chips for 2 big chips or 4 small chips)
- Wait time, only x fryers, if in use, have to wait for them to be cooked
- Dine in options (more expensive)
- Side, rolls, peas, gravy, etc
- Staffing (requirements of staff to be on-shift)
- Multi discount (e.g. multiple pie offer the best discount)


