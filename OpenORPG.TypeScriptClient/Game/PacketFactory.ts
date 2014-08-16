module PacketFactory {

    export function createLoginPacket(user: string, password: string) {
        return {
            opCode: OpenORPG.OpCode.CMSG_LOGIN_REQUEST,
            username: user,
            password: password
        };
    }

    export function createHeroSelectPacket(id: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_HERO_SELECT,
            heroId: id
        };
    }


    export function createMovementPacket(x: number, y: number, terminate: boolean, direction: OpenORPG.Direction) {
        return {
            opCode: OpenORPG.OpCode.CMSG_MOVEMENT_REQUEST,
            currentPosition: {
                x: x,
                y: y
            },

            terminates: terminate,
            direction: direction
        }
    }

    export function createZoneRequestChange(dir: OpenORPG.Direction): any {
        return {
            opCode: OpenORPG.OpCode.CMMSG_ZONE_CHANGE,
            direction: dir
        }
    }

    export function createSkillUsePacket(skillId: number, targetId: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_USE_SKILL,
            skillId: skillId,
            targetId: targetId
        }
    }

    export function createInteractionRequest() {
         return {
             opCode: OpenORPG.OpCode.CMSG_INTERACT_REQUEST,
             data: "Something to fill up space"
        }
    }


    export function createChatPacket(channeld: number, message: string) {
        return {
            opCode: OpenORPG.OpCode.CMSG_CHAT_MESSAGE,
            channelId: channeld,
            message: message
        }
    }

    export function createStorageMoveRequest(source: number, dest: number, stype: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_STORAGE_MOVE_SLOT,
            sourceSlot: source,
            destSlot: dest,
            type: stype
        }
       
    }

    export function createStorageDropRequest(slotId: number, amount: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_STORAGE_DROP,
            slotId: slotId,
            amount: amount
        }
    }

    export function createItemuseRequest(slotId: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_ITEM_USE,
            slotId: slotId
        }
    }

    export function createUnEqupRequest(slot: OpenORPG.EquipmentSlot) {
        return {
            opCode: OpenORPG.OpCode.CMSG_UNEQUIP_ITEM,
            slot: slot
        }
    }

    export function createQuestAcceptRequest(questId: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_QUEST_ACCEPT,
            questId: questId
        }
    }

    export function createTargetNotification(targetId: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_ENTITY_TARGET,
            entityId: targetId
        }
    }


} 