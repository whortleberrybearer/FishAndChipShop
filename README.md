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