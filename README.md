# Poultry Farm Management System

## Project Description

This project is a software system designed to assist in the administration of a poultry farm. The system manages information about farm employees and chickens housed on the farm.

## Chicken Information

The system stores the following details for each chicken:
- **Weight**  
- **Age**  
- **Number of eggs laid monthly**  
- **Location** (specific cage assigned to the chicken)

Each chicken is assigned to a unique cage.

## Employee Information

The system also tracks information about farm employees:
- **Passport details**  
- **Salary**  
- **Assigned cages**

## Business Rules

- Each chicken must be serviced by at least one employee.  
- The number of chickens may increase or decrease over time.  
- Some cages may remain empty at certain moments.  
- The price of eggs is the same for all chickens.  
- Dates are limited to the days within a single month.

## Key Entities and Attributes

- **Chicken**:
  - Weight
  - Age
  - Egg-laying rate (productivity)
  - Breed
  - Cage location
  - Egg-laying status (laid/not laid)
  
- **Employee**:
  - Full name
  - Salary
  - Assigned cages

- **Cage**:
  - Unique ID

## Generalized Lists

The system will include the following lists:
- Chickens  
- Poultry farm  
- Employees  

## Reports for the Director

The system must generate the following reports:
1. **Average number of eggs**: Produced by chickens of a specific weight and age.  
2. **Total eggs and value**: Total number of eggs produced over a date range and their total monetary value.  
3. **Employee performance**: Number of eggs collected by each employee.  
4. **Underperforming chickens**: Chickens that laid fewer eggs than the farm's average productivity.  
5. **Top-performing chicken**: The cage of the chicken that laid the most eggs.  
6. **Employee assignments**: Number of chickens assigned to each employee.  

## Additional Features

- Ability to add and remove chickens.  

---
