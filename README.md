**Dynamic Exposure Rebalancing**

This project demonstrates a **portfolio exposure rebalancing algorithm** for entities, respecting capacity constraints and priority rules while minimizing redistribution.


--Problem Statement

Redistribute "exposures" between entities in a portfolio to satisfy constraints.  

Each entity has:

- EntityId: Unique identifier  
- Exposure: Current exposure amount (positive decimal)  
- Capacity: Maximum allowable exposure  
- Priority: Rank indicating importance (lower = higher priority)  

Implement a method Rebalance() that:

1. Ensures no entity exceeds capacity  
2. Maintains total exposure  
3. Minimizes redistributions  
4. Prioritizes lower-priority entities to receive redistributed exposure  
5. Handles not-possible scenarios without exceptions  

--Features
- Rebalance exposures respecting capacity and priority  
- Graceful handling when redistribution is not possible
- Maintains total portfolio exposure 
- Unit-tested using xUnit  


--Rebalabncing Logic

Start Rebalance ->  Identify entities exceeding capacity -> Calculate excess exposure ->  Sort entities by priority  ->
    
-> Redistribute excess exposure to eligible entity within its capacity -> (Yes) Rebalance sucess  OR (No) Rebalance fail
          

- Used .NET 9.0 SDK 
- Run Console App
- You can modify Program.cs to test different entities and capacities
- Program.cs has two ways to test using sync and async rebalance with different entities, commented  to avoid confusion. Can be tested by uncommenting.
- Run Unit tests