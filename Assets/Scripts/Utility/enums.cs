[System.Serializable()]
public enum Item { bamboo, book, coat, food, knife, pin, sake, toy, fox_mask, boar_mask, ox_mask, tiger_mask }

[System.Serializable()]
public enum Character { pc, prentice, kitchen_hand, smith, chef, laundress, groundskeeper, librarian, guard, lazy_guard }

[System.Serializable()]
public enum Action_Type { give, take }

public enum Game_state { talking, walking };
public enum Item_state {free, inventory, in_use};
public enum Use_Type { pickup, inspect, talkto, gothrough, use }