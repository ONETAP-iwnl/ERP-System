'use client'

import { useRouter } from 'next/navigation';
import { useState } from "react"
import styles from "@/styles/recipts-page/hero/hero.module.css"
import Hero from '@/components/resource/create/hero/hero';

export default function Create() {
    return(
        <Hero></Hero>
    )
}